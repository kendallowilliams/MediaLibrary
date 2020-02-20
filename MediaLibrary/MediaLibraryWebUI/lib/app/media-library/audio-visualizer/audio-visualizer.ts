import BaseClass from "../../assets/models/base-class";
import PlayerConfiguration from "../../assets/models/configurations/player-configuration";
import HtmlControls from "../../assets/controls/html-controls";

export default class AudioVisualizer extends BaseClass {
    private canvas: HTMLCanvasElement;
    private canvasContext: CanvasRenderingContext2D;
    private audioElement: HTMLAudioElement;
    private audioContext: AudioContext;
    private analyser: AnalyserNode;
    private dataArray: Uint8Array;
    private bufferLength: number;
    private previousDataArray: Uint8Array;
    private audioSourceNode: MediaElementAudioSourceNode;
    private fftSize: number;

    constructor(private playerConfiguration: PlayerConfiguration, audioElement: HTMLAudioElement) {
        super();
        this.audioElement = audioElement as HTMLAudioElement;
        this.canvas = HtmlControls;
        this.containerHeight = () => $(this.canvas).parent().height();
        this.containerWidth = () => $(this.canvas).parent().width();
        this.canvasContext = this.canvas.getContext('2d');
        this.init();
    }

    init() {
        this.audioContext = new (window.AudioContext || window.webkitAudioContext)();
        this.analyser = this.audioContext.createAnalyser();
        this.source = this.audioContext.createMediaElementSource(this.mediaSource);

        this.analyser.fftSize = 256;
        this.bufferLength = this.analyser.frequencyBinCount;
        this.dataArray = new Uint8Array(this.bufferLength);
        this.previousDataArray = new Uint8Array(this.bufferLength);
        this.source.connect(this.audioContext.destination);
        this.source.connect(this.analyser);
        this.analyser.connect(this.audioContext.destination);
    }

    clear(width, height) {
        this.canvasContext.clearRect(0, 0, width, height);
    }

    static prepareCanvas(canvas, width, height) {
        var canvasContext = canvas.getContext('2d');

        canvas.width = width;
        canvas.height = height;
        canvasContext.fillStyle = 'rgb(200, 200, 200)';
        canvasContext.fillRect(0, 0, width, height);
    }

    draw() {
        var width = this.containerWidth(),
            height = this.containerHeight(),
            numberOfBars = 128,
            barWidth = this.canvas.width / numberOfBars,
            barHeight = 0,
            discY = 0,
            discHeight = 5,
            x = 0,
            step = Math.floor(this.bufferLength / numberOfBars);

        this.clear(this.canvas.width, this.canvas.height);
        if (this.analyser) /*then*/ this.analyser.getByteFrequencyData(this.dataArray);
        AudioVisualizer.prepareCanvas(this.canvas, width, height);
        for (var i = 0; i < this.previousDataArray.length && !this.playerStopped; i++) {
            if (this.dataArray[i] > this.previousDataArray[i]) {
                this.previousDataArray[i] = this.dataArray[i];
            } else if (this.previousDataArray[i] > 0) {
                this.previousDataArray[i] -= 1;
            }
        }
        for (var i = 0; i < numberOfBars && !this.playerStopped; i++) {
            barHeight = this.dataArray[i * step] * Math.floor(height / @(byte.MaxValue));
            discY = (this.previousDataArray[i * step] * Math.floor(height / @(byte.MaxValue))) + discHeight;
            this.canvasContext.fillStyle = 'rgb(' + (barHeight + 100) + ',50,50)';
            this.canvasContext.fillRect(x, height - barHeight, barWidth - 1, barHeight);
            this.canvasContext.fillStyle = 'white';
            this.canvasContext.fillRect(x, height - discY - 1, barWidth - 1, discHeight);
            x += barWidth;
        }

        if (this.playerStopped) {
            this.reset();
        }
        else window.requestAnimationFrame(this.draw.bind(this));
    }

    reset() {
        var width = this.containerWidth(),
            height = this.containerHeight(),
            numberOfBars = 128,
            barWidth = this.canvas.width / numberOfBars,
            barHeight = 0,
            discY = 0,
            discHeight = 5,
            x = 0,
            step = Math.floor(this.bufferLength / numberOfBars);

        this.clear(this.canvas.width, this.canvas.height);
        AudioVisualizer.prepareCanvas(this.canvas, width, height);

        for (var i = 0; i < numberOfBars; i++) {
            barHeight = this.dataArray[i * step] * Math.floor(height / @(byte.MaxValue));
            discY = (this.previousDataArray[i * step] * Math.floor(height / @(byte.MaxValue))) + discHeight;
            if (this.dataArray[i * step] > 0) /*then*/ this.dataArray[i * step] -= 1;
            if (this.previousDataArray[i * step] > 0) /*then*/ this.previousDataArray[i * step] -= 1;

            if (barHeight > 0) {
                this.canvasContext.fillStyle = 'rgb(' + (barHeight + 100) + ',50,50)';
                this.canvasContext.fillRect(x, height - barHeight, barWidth - 1, barHeight);
            }

            if (discY >= discHeight) {
                this.canvasContext.fillStyle = 'white';
                this.canvasContext.fillRect(x, height - discY - 1, barWidth - 1, discHeight);
            }
            x += barWidth;
        }

        if (this.dataArray.find((value, index) => value > 0) ||
            this.previousDataArray.find((value, index) => value > 0)) /*then*/ window.requestAnimationFrame(this.reset.bind(this));
    }

    start() {
        if (this.audioContext) {
            this.playerStopped = false;
            this.draw();
        }
    }

    pause() {
        this.playerStopped = true;
    }

    close() {
        this.pause();
        if (this.audioContext.state !== 'closed') /*then*/ this.audioContext.close()
        this.analyser = this.source = this.audioContext = null;
    }
}

}