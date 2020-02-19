import { PodcastPages, PodcastSort, PodcastFilter } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IPodcastConfiguration extends IConfiguration {
    selectedPodcastId: number;
    selectedPodcastPage: PodcastPages;
    selectedPodcastSort: PodcastSort;
    selectedPodcastFilter: PodcastFilter;
}