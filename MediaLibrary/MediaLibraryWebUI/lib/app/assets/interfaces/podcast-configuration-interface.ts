import { PodcastPages, PodcastSort, PodcastFilter } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IPodcastConfiguration extends IConfiguration {
    SelectedPodcastId: number;
    SelectedPodcastPage: PodcastPages;
    SelectedPodcastSort: PodcastSort;
    SelectedPodcastFilter: PodcastFilter;
}