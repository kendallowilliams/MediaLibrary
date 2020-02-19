import { MediaTypes, RepeatTypes, PlayerPages } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IPlayerConfiguration extends IConfiguration {
    selectedMediaType: MediaTypes;
    currentItemIndex: number;
    autoPlay: boolean;
    repeat: RepeatTypes;
    shuffle: boolean;
    selectedPlayerPage: PlayerPages;
    volume: number;
    muted: boolean;
    audioVisualizerEnabled: boolean;
}