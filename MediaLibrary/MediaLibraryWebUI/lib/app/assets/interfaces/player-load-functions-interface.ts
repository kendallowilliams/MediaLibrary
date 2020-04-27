export default interface IPlayerLoadFunctions {
    loadArtist: (id) => void,
    loadAlbum: (id) => void,
    loadPodcast: (id) => void,
    loadSeries: (id) => void
}