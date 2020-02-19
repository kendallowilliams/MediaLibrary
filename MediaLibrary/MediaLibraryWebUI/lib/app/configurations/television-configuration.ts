import BaseConfiguration from './base-configuration'
import * as Enums from '../assets/enums'

export default class TelevisionConfiguration extends BaseConfiguration {
    private selectedSeriesId: number;
    private selectedSeason: number;
    private selectedTelevisionPage: Enums.TelevisionPages;
    private selectedSeriesSort: Enums.SeriesSort;

    constructor() {
        super();
    }
}
