import { MediaPages } from "../enums/enums";
import IConfiguration from "./configuration-interface";

export default interface IMediaLibraryConfiguration extends IConfiguration {
    selectedMediaPage: MediaPages;
}