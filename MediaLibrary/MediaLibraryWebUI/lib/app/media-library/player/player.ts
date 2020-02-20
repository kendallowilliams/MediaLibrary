import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlayerConfiguration from "../../assets/models/configurations/player-configuration";
import HtmlControls from '../../assets/controls/html-controls'
import { MediaTypes, RepeatTypes, PlayerPages } from "../../assets/enums/enums";
import { getRandomInteger } from "../../assets/utilities/math";
import AudioVisualizer from "../audio-visualizer/audio-visualizer";

export default class Player extends BaseClass implements IView {
    private players: { VideoPlayer: HTMLMediaElement, MusicPlayer: HTMLMediaElement };
    private unPlayedShuffleIds: string[];
    private audioVisualizer: AudioVisualizer;

    constructor(private playerConfiguration: PlayerConfiguration) {
        super();
        this.players = HtmlControls.Players();
        this.unPlayedShuffleIds = [];
        this.audioVisualizer = new AudioVisualizer(playerConfiguration);
    }

    loadView(): void {
    }

    private initMediaPlayers(): void {
        $(this.getPlayers()).on('ended', e => this.updatePlayCount(e.target as HTMLMediaElement, this.loadNext));
        $(this.getPlayers()).prop('volume', this.playerConfiguration.properties.Volume / 100.0);

        $(this.getPlayers()).on('durationchange', e => {
            const player: HTMLMediaElement = e.target as HTMLMediaElement;

            $('#@(HtmlControlsRepository.PlayerSliderId)').slider('option', 'max', player.duration);
            $('#@(HtmlControlsRepository.PlayerTimeId)').text(this.getPlaybackTime(player.currentTime, player.duration));
        });

        $(this.getPlayers()).on('timeupdate', e => {
            const player: HTMLMediaElement = e.target as HTMLMediaElement;

            this.enableDisablePreviousNext();
            if ($('#@(HtmlControlsRepository.PlayerSliderId)').attr('data-slide-started') !== 'true') {
                $('#@(HtmlControlsRepository.PlayerSliderId)').slider('value', Math.floor(player.currentTime));
                $('#@(HtmlControlsRepository.PlayerTimeId)').text(this.getPlaybackTime(player.currentTime, player.duration));
            }
        });

        $(this.getPlayers()).on('play', e => {
            const mediaType = this.playerConfiguration.properties.SelectedMediaType,
                audioVisualizerEnabled = this.playerConfiguration.properties.AudioVisualizerEnabled;

            if (this.getPlayer().duration === Infinity) /*then*/ this.getPlayer().src = this.getPlayer().src;
            $(e.target).attr('data-playing', 'true');
            $('#@(HtmlControlsRepository.HeaderPlayButtonId), #@(HtmlControlsRepository.PlayerPlayButtonId)').addClass('d-none');
            $('#@(HtmlControlsRepository.HeaderPauseButtonId), #@(HtmlControlsRepository.PlayerPauseButtonId)').removeClass('d-none');
            this.initAudioVisualizer();
            if (this.audioVisualizer && mediaType !== MediaTypes.Television && audioVisualizerEnabled) /*then*/ this.audioVisualizer.start();
        });

        $(this.getPlayers()).on('pause', e => {
            $('#@(HtmlControlsRepository.HeaderPauseButtonId), #@(HtmlControlsRepository.PlayerPauseButtonId)').addClass('d-none');
            $('#@(HtmlControlsRepository.HeaderPlayButtonId), #@(HtmlControlsRepository.PlayerPlayButtonId)').removeClass('d-none');
            if (this.audioVisualizer && this.playerConfiguration.properties.AudioVisualizerEnabled) /*then*/ this.audioVisualizer.pause();
        });

        $(this.getPlayers()).on('error', e => null);
    }

    private initPlayerControls(): void {
        var $volumeSlider = $('<div id="@(HtmlControlsRepository.VolumeSliderId)"></div>').addClass('m-1');

        $('#@(HtmlControlsRepository.PlayerSliderId)').slider({ min: 0, max: 100 });
        $volumeSlider.slider({
            min: 0,
            max: 100,
            orientation: 'vertical',
            value: this.playerConfiguration.properties.Muted ? 0 : this.playerConfiguration.properties.Volume
        });
        $('#@(HtmlControlsRepository.PlayerVolumeContainerId)').popover({
            trigger: 'hover',
            content: $volumeSlider[0],
            placement: 'top',
            html: true,
            container: $('#@(HtmlControlsRepository.PlayerVolumeContainerId)')[0]
        });
        $volumeSlider.on('slide', (e, ui) => {
            var volume = ui.value;

            $('#@(HtmlControlsRepository.PlayerVolumeButtonId), #@(HtmlControlsRepository.PlayerMuteButtonId)').attr('data-volume', volume).addClass('d-none');
            $(volume == 0 ? '#@(HtmlControlsRepository.PlayerMuteButtonId)' : '#@(HtmlControlsRepository.PlayerVolumeButtonId)').removeClass('d-none');
            this.playerConfiguration.properties.Volume = volume;
        this.playerConfiguration.properties.Muted = volume == 0;
            $('#@(HtmlControlsRepository.MusicPlayerId), #@(HtmlControlsRepository.VideoPlayerId)').prop('volume', volume / 100.0).prop('muted', volume == 0)
        });
        $volumeSlider.on('slidechange', (e, ui) => {
            this.playerConfiguration.updateConfiguration();
        });
        $('#@(HtmlControlsRepository.PlayerSliderId)').on('slide', (e, ui) => {
            if ($(e.target).attr('data-slide-started') === 'true') {
                $(this.getPlayer()).prop('currentTime', ui.value);
                $('#@(HtmlControlsRepository.PlayerTimeId)').text(this.getPlaybackTime(ui.value, $(e.target).slider('option', 'max')));
            }
        });
        $('#@(HtmlControlsRepository.PlayerSliderId)').on('slidestart', (e, ui) => $(e.target).attr('data-slide-started', 'true'));
        $('#@(HtmlControlsRepository.PlayerSliderId)').on('slidestop', (e, ui) => $(e.target).attr('data-slide-started', 'false'));
        $('#@(HtmlControlsRepository.HeaderNextButtonId), #@(HtmlControlsRepository.PlayerNextButtonId)').on('click', () => this.loadNext());
        $('#@(HtmlControlsRepository.HeaderPreviousButtonId), #@(HtmlControlsRepository.PlayerPreviousButtonId)').on('click', () => this.loadPrevious());
        $('#@(HtmlControlsRepository.HeaderPauseButtonId), #@(HtmlControlsRepository.PlayerPauseButtonId)').on('click', () => $(this.getPlayer()).attr('data-playing', 'false').trigger('pause'));
        $('#@(HtmlControlsRepository.HeaderPlayButtonId), #@(HtmlControlsRepository.PlayerPlayButtonId)').on('click', () => {
            if (this.getPlayer().currentSrc) /*then*/ $(this.getPlayer()).trigger('play');
        });
        $('#@(HtmlControlsRepository.HeaderShuffleButtonId), #@(HtmlControlsRepository.PlayerShuffleButtonId)').addClass(this.playerConfiguration.properties.Shuffle ? 'active' : '');
        $('button[data-repeat-type]').on('click', () => {
            let repeat = this.playerConfiguration.properties.Repeat;

            $('button[data-repeat-type]').addClass('d-none');

            if (repeat === RepeatTypes.None) {
                repeat = RepeatTypes.RepeatOne;
            } else if (repeat === RepeatTypes.RepeatOne) {
                repeat = RepeatTypes.RepeatAll;
            } else if (repeat === RepeatTypes.RepeatAll) {
                repeat = RepeatTypes.None;
            }

            $('button[data-repeat-type="' + repeat + '"]').removeClass('d-none');
            this.playerConfiguration.properties.Repeat = repeat;
            this.playerConfiguration.updateConfiguration(() => this.enableDisablePreviousNext());
        });
        $('#@(HtmlControlsRepository.HeaderShuffleButtonId), #@(HtmlControlsRepository.PlayerShuffleButtonId)').on('click', () => {
            var shuffle = this.playerConfiguration.properties.Shuffle,
                $btns = $('#@(HtmlControlsRepository.HeaderShuffleButtonId), #@(HtmlControlsRepository.PlayerShuffleButtonId)');
            
            this.setUnPlayedShuffleIds(!shuffle);
            this.playerConfiguration.properties.Shuffle = !shuffle;
            this.playerConfiguration.updateConfiguration(() => {
                if (!shuffle) {
                    $btns.addClass('active');
                } else {
                    $btns.removeClass('active');
                }
                this.enableDisablePreviousNext();
            });
        });
        $('#@(HtmlControlsRepository.PlayerVolumeButtonId), #@(HtmlControlsRepository.PlayerMuteButtonId)').on('click', e => {
            let previousVolume = parseInt($('#@(HtmlControlsRepository.PlayerVolumeButtonId)').attr('data-volume')),
                $btn = $(e.currentTarget),
                muted = false;

            $('#@(HtmlControlsRepository.PlayerVolumeButtonId), #@(HtmlControlsRepository.PlayerMuteButtonId)').addClass('d-none');

            if ($btn.attr('id') === '@(HtmlControlsRepository.PlayerVolumeButtonId)') {
                $('#@(HtmlControlsRepository.PlayerMuteButtonId)').removeClass('d-none');
                $volumeSlider.slider('value', 0);
                muted = true;
            } else if ($btn.attr('id') === '@(HtmlControlsRepository.PlayerMuteButtonId)') {
                $('#@(HtmlControlsRepository.PlayerVolumeButtonId)').removeClass('d-none');
                $volumeSlider.slider('value', previousVolume);
            }

            this.playerConfiguration.properties.Muted = muted;
            this.playerConfiguration.updateConfiguration(() => $(this.getPlayers()).each((index, element) => { (element as HTMLAudioElement).muted = muted; }));
        });
        $('#@(HtmlControlsRepository.PlayerFullscreenButtonId)').on('click', () => openFullscreen(this.getPlayer()));
        $('button[data-repeat-type="' + this.getRepeatTypesEnumString(this.playerConfiguration.properties.Repeat) + '"]').removeClass('d-none');
    }

    private initAudioVisualizer(): void {

    }

    loadItem(item: HTMLElement, triggerPlay: boolean): void {
        const $player = $(this.getPlayer()),
            shuffleEnabled = this.playerConfiguration.properties.Shuffle;

        $(this.getPlayers()).prop('src', '').attr('data-item-id', '');

        if (item) {
            let $item = $(item),
                url = $item.attr('data-play-url'),
                index = parseInt($item.attr('data-play-index')),
                id = $item.attr('data-item-id'),
                title = $item.attr('data-title') || '';

            $('li[data-play-index].list-group-item').removeClass('active');
            this.playerConfiguration.properties.CurrentItemIndex = index;
            this.playerConfiguration.updateConfiguration(function () {
                $item.addClass('active');
                $player.attr('data-item-id', id);
                $('#@(HtmlControlsRepository.NowPlayingTitleId)').text(title.length > 0 ? ': ' + title : title);
                if (shuffleEnabled && $.inArray(index, this.unPlayedShuffleIds) >= 0) /*then*/ this.unPlayedShuffleIds.splice(this.unPlayedShuffleIds.indexOf(index), 1);
                this.updateScrollTop();
                $player.prop('src', url);
                if (triggerPlay) /*then*/ $player.trigger('play');
                this.enableDisablePreviousNext();
            });
        } else if ($('li[data-play-index].active').length === 1) {
            this.loadItem($('li[data-play-index].active')[0], triggerPlay);
        }
    }

    private loadNext(): void {
        var shuffle = this.playerConfiguration.properties.Shuffle,
            nextIndex = shuffle ? this.unPlayedShuffleIds[getRandomInteger(0, this.unPlayedShuffleIds.length - 1)] :
                this.playerConfiguration.properties.CurrentItemIndex + 1,
            $item = null,
            repeat = this.playerConfiguration.properties.Repeat,
            shuffleEmpty = this.unPlayedShuffleIds.length == 0;

        if (repeat === RepeatTypes.RepeatOne) {
            $(this.getPlayer()).prop('currentTime', 0);
        } else if (repeat === RepeatTypes.RepeatAll) {
            if (shuffle && shuffleEmpty) {
                this.setUnPlayedShuffleIds(shuffle);
                this.loadNext();
            }
            else if (nextIndex === $('li[data-play-index]').length) {
                $item = $('li[data-play-index="0"]');
                this.loadItem($item[0], this.isPlaying());
            } else {
                $item = $('li[data-play-index="' + nextIndex + '"]');
                this.loadItem($item[0], this.isPlaying());
            }
        } else {
            $item = $('li[data-play-index=' + nextIndex + ']');

            if ((shuffle && !shuffleEmpty) || (!shuffle && nextIndex < $('li[data-play-index]').length)) {
                this.loadItem($item[0], this.isPlaying());
            } else {
                $('#@(HtmlControlsRepository.PlayerPauseButtonId)').trigger('click');
                this.enableDisablePreviousNext();
            }
        }
    }

    private loadPrevious(): void {
        var shuffle = this.playerConfiguration.properties.Shuffle,
            previousIndex = shuffle ? this.unPlayedShuffleIds[getRandomInteger(0, this.unPlayedShuffleIds.length - 1)] :
                this.playerConfiguration.properties.CurrentItemIndex - 1,
            $item = $('li[data-play-index="' + previousIndex + '"]'),
            player = this.getPlayer(),
            shuffleEmpty = this.unPlayedShuffleIds.length == 0,
            repeat = this.playerConfiguration.properties.Repeat;

        if (repeat === RepeatTypes.RepeatOne || player.currentTime > 5) {
            player.currentTime = 0;
        }
        else if (shuffle && shuffleEmpty) {
            this.setUnPlayedShuffleIds(shuffle);
            this.loadPrevious();
        }
        else this.loadItem($item[0], this.isPlaying());
    }

    private canPlayNext(): boolean {
        return (this.playerConfiguration.properties.Shuffle && this.unPlayedShuffleIds.length > 0) ||
            this.playerConfiguration.properties.Repeat !== RepeatTypes.None ||
            this.playerConfiguration.properties.CurrentItemIndex < ($('li[data-play-index]').length - 1);
    }

    private canPlayPrevious(): boolean {
        return this.playerConfiguration.properties.Shuffle ||
            this.playerConfiguration.properties.CurrentItemIndex > 0 ||
            this.getPlayer().currentTime > 5 ||
            this.playerConfiguration.properties.Repeat === RepeatTypes.RepeatAll;
    }

    private isPlaying(): boolean {
        return $(this.getPlayer()).attr('data-playing') === 'true';
    }

    private getPlayer(): HTMLAudioElement | HTMLVideoElement {
        return this.playerConfiguration.properties.SelectedMediaType === MediaTypes.Television ?
            this.players.VideoPlayer as HTMLVideoElement :
            this.players.MusicPlayer as HTMLAudioElement;
    }

    private getPlayers(): HTMLElement[] { return [this.players.MusicPlayer, this.players.VideoPlayer]; }

    private updateScrollTop(): void {
        const $item = $('li[data-play-index].active');

        if ($item.length > 0) {
            const container = HtmlControls.Containers().PlayerItemsContainer;

            $(container).scrollTop($(container).scrollTop() - $item.position().top * -1);
        }
    }

    private getPlaybackTime(time, duration): string {
        return this.getFormattedTime(time).concat('/').concat(this.getFormattedTime(duration));
    }

    private getFormattedTime(time): string {
        let adjustedTime = Number.isNaN(time) || !Number.isFinite(time) ? 0 : time,
            currentHours = Math.floor(adjustedTime / 3600),
            currentMinutes = Math.floor((adjustedTime - (currentHours * 3600)) / 60),
            currentSeconds = Math.floor((adjustedTime - (currentMinutes * 60 + currentHours * 3600)) % 60),
            currentTime = (currentHours > 0 ? currentHours.toString().padStart(2, '0').concat(':') : '')
                .concat(currentMinutes.toString().padStart(2, '0').concat(':'))
                .concat(currentSeconds.toString().padStart(2, '0'));

        return currentTime;
    }

    private setUnPlayedShuffleIds(shuffle: boolean): void {
        const $items = $('li[data-play-index]');

        this.unPlayedShuffleIds = shuffle && $items.length > 0 ? $.makeArray($items.map((index, element) => $(element).attr('data-play-index'))) : [];
    }

    private enableDisablePreviousNext(): void {
        const buttons = HtmlControls.Buttons(),
            nextButtons = [buttons.HeaderNextButton, buttons.PlayerNextButton],
            previousButtons = [buttons.HeaderPreviousButton, buttons.PlayerPreviousButton];

        $(nextButtons).prop('disabled', !this.canPlayNext());
        $(previousButtons).prop('disabled', !this.canPlayPrevious());
    }

    private updatePlayCount(player: HTMLMediaElement, callback: () => void = () => null) {
        const id = $(player).attr('data-item-id');

        $.post('/Player/UpdatePlayCount', { mediaType: this.playerConfiguration.properties.SelectedMediaType, id: id }, callback);
    }

    private reload(callback: () => void = () => null): void {
        const success = () => {
            loadTooltips($('#@(HtmlControlsRepository.PlayerItemsContainerId)')[0]);
            this.updateSelectedPlayerPage();
            if (typeof callback === 'function') /*then*/ callback();
        };

        $('#@(HtmlControlsRepository.PlayerItemsContainerId)').html('');
        $('#@(HtmlControlsRepository.PlayerItemsContainerId)').load('@(Url.Action("GetPlayerItems", "Player"))', success);
    }

    private togglePlaylist(btn: HTMLButtonElement): void {
        let page = this.playerConfiguration.properties.SelectedPlayerPage,
        $player = $(this.getPlayer()),
            $playerItems = $('#@(HtmlControlsRepository.PlayerItemsContainerId)'),
            $btn = $(btn);

        $('#@(HtmlControlsRepository.PlayerFullscreenButtonId)').addClass('d-none');
        if (page === PlayerPages.Index) {
            this.playerConfiguration.properties.SelectedPlayerPage = this.getPlayerPageEnum($player.attr('data-player-page'));
            $player.parent().removeClass('d-none');
            $playerItems.addClass('d-none');
            $btn.removeClass('active');
            page = this.playerConfiguration.properties.SelectedPlayerPage;
            if (page === PlayerPages.Video) /*then*/ $('#@(HtmlControlsRepository.PlayerFullscreenButtonId)').removeClass('d-none');
            else if (page === PlayerPages.Audio) /*then*/ this.initCanvas();
        } else {
            this.playerConfiguration.properties.SelectedPlayerPage = PlayerPages.Index;
            $player.parent().addClass('d-none');
            $playerItems.removeClass('d-none');
            $btn.addClass('active');
        }
        this.playerConfiguration.updateConfiguration();
    }

    private updateSelectedPlayerPage(): void {
        let selectedMediaType = this.playerConfiguration.properties.SelectedMediaType,
            selectedPlayerPage = this.playerConfiguration.properties.SelectedPlayerPage;

        if (selectedMediaType === MediaTypes.Television && selectedPlayerPage === PlayerPages.Audio) {
            this.playerConfiguration.properties.SelectedPlayerPage = PlayerPages.Video;
        } else if (selectedMediaType !== MediaTypes.Television && selectedPlayerPage === PlayerPages.Video) {
            this.playerConfiguration.properties.SelectedPlayerPage = PlayerPages.Audio;
        }

        if (selectedPlayerPage !== this.playerConfiguration.properties.SelectedPlayerPage) {
            selectedPlayerPage = this.playerConfiguration.properties.SelectedPlayerPage;
            $('#@(HtmlControlsRepository.PlayerFullscreenButtonId)').addClass('d-none');

            this.playerConfiguration.updateConfiguration(() =>
                $(this.getPlayers()).each((index, element) => {
                    const page = $(element).attr('data-player-page');

                    if (this.getPlayerPageEnum(page) === selectedPlayerPage) /*then*/ $(element).parent().removeClass('d-none');
                    else $(element).parent().addClass('d-none');
                })
            );

            if (selectedPlayerPage === PlayerPages.Video) /*then*/ $('#@(HtmlControlsRepository.PlayerFullscreenButtonId)').removeClass('d-none');
        }
    }

    private getPlayerPageEnum(page: string): PlayerPages {
        let playerPage: PlayerPages = PlayerPages.Index;

        switch (page) {
            case 'Audio':
                playerPage = PlayerPages.Audio;
                break;
            case 'Video':
                playerPage = PlayerPages.Video;
                break;
            case 'Index':
            default:
                playerPage = PlayerPages.Index;
                break;
        }

        return playerPage;
    }

    private getRepeatTypesEnumString(page: RepeatTypes): string {
        let repeatType: string = '';

        switch (page) {
            case RepeatTypes.None:
                repeatType = 'None';
                break;
            case RepeatTypes.RepeatAll:
                repeatType = 'RepeatAll';
                break;
            case RepeatTypes.RepeatOne:
                repeatType = 'RepeatOne';
                break;
            default:
                repeatType = '';
                break;
        }

        return repeatType;
    }
}