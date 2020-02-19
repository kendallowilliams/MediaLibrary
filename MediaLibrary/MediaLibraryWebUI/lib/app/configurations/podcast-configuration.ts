﻿import BaseConfiguration from './base-configuration'
import * as Enums from '../assets/enums'

export default class PodcastConfiguration extends BaseConfiguration {
    private selectedPodcastId: number;
    private selectedPodcastPage: Enums.PodcastPages;
    private selectedPodcastSort: Enums.PodcastSort;
    private selectedPodcastFilter: Enums.PodcastFilter;

    constructor() {
        super();
    }
}
