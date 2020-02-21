import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import TelevisionConfiguration from "../../assets/models/configurations/television-configuration";
import HtmlControls from '../../assets/controls/html-controls';
import { TelevisionPages } from "../../assets/enums/enums";
import ITelevisionConfiguration from "../../assets/interfaces/television-configuration-interface";

export default class Television extends BaseClass implements IView {
    private readonly mediaView: HTMLElement;

    constructor(private televisionConfiguration: TelevisionConfiguration, private playFunc: (btn: HTMLButtonElement) => void) {
        super();
        this.mediaView = HtmlControls.Views().MediaView;
    }

    loadView(callback: () => void = () => null): void {
        const properties: ITelevisionConfiguration = this.televisionConfiguration.properties,
            success: () => void = () => {
                this.initializeControls();
                callback();
            };

        $(this.mediaView).load('/Television/Index', success);
    }

    initializeControls(): void {
        $('[data-play-id]').on('click', e => this.playFunc(e.target as HTMLButtonElement));
        $('[data-back-button="television"]').on('click', () => this.goBack(() => this.loadView.call(this)));
    }

    loadSeries(id: number, callback: () => void = () => null): void {
        this.televisionConfiguration.properties.SelectedSeriesId = id;
        this.televisionConfiguration.properties.SelectedTelevisionPage = TelevisionPages.Series;
        this.televisionConfiguration.updateConfiguration(callback);
    }

    goBack(callback: () => void = () => null): void {
        this.televisionConfiguration.properties.SelectedSeriesId = 0;
        this.televisionConfiguration.properties.SelectedTelevisionPage = TelevisionPages.Index;
        this.televisionConfiguration.updateConfiguration(callback);
    }
}