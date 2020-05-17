import HomeConfig from './home-configuration';
import MediaLibraryConfig from './media-library-configuration';
import MusicConfig from './music-configuration';
import PlayerConfig from './player-configuration';
import PlaylistConfig from './playlist-configuration';
import PodcastConfig from './podcast-configuration';
import TelevisionConfig from './television-configuration';

export default {
    Home: json => new HomeConfig(json),
    MediaLibrary: json => new MediaLibraryConfig(json),
    Music: json => new MusicConfig(json),
    Player: json => new PlayerConfig(json),
    Playlist: json => new PlaylistConfig(json),
    Podcast: json => new PodcastConfig(json),
    Television: json => new TelevisionConfig(json)
};