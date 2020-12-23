import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlayerConfiguration from "../../assets/models/configurations/player-configuration";
import HtmlControls from '../../assets/controls/html-controls'
import { MediaTypes, RepeatTypes, PlayerPages } from "../../assets/enums/enums";
import { getRandomInteger } from "../../assets/utilities/math";
import AudioVisualizer from "../audio-visualizer/audio-visualizer";
import { openFullscreen } from "../../assets/utilities/element";
import { loadTooltips, disposeTooltips } from "../../assets/utilities/bootstrap-helper";
import LoadingModal from '../../assets/modals/loading-modal';
import IPlayerLoadFunctions from "../../assets/interfaces/player-load-functions-interface";
import ClearNowPlayingModal from "../../assets/modals/clear-now-playing-modal";
import { getRepeatTypesEnumString, getPlayerPageEnum, getMediaTypesEnumString, getMediaTypesEnum } from "../../assets/enums/enum-functions";

export default class Player extends BaseClass implements IView {
    private players: { VideoPlayer: HTMLMediaElement, MusicPlayer: HTMLMediaElement };
    private unPlayedShuffleIds: number[];
    private audioVisualizer: AudioVisualizer;
    private playerView: HTMLElement;
    private clearNowPlayingModal: ClearNowPlayingModal;
    private currentlyLoadedId: number;

    constructor(private playerConfiguration: PlayerConfiguration, private loadFunctions: IPlayerLoadFunctions, private updateActiveMedia: () => void = () => null) {
        super();
        this.players = HtmlControls.Players();
        this.playerView = HtmlControls.Views().PlayerView;
        this.unPlayedShuffleIds = [];
        this.audioVisualizer = new AudioVisualizer(this.playerConfiguration, this.players.MusicPlayer);
        this.clearNowPlayingModal = new ClearNowPlayingModal(() => this.reload(() => this.loadItem()));
        this.initPlayer();
        this.currentlyLoadedId = 0;
    }

    loadView(callback: () => void = () => null): void {
        this.audioVisualizer.prepareCanvas();
        this.updateScrollTop();
        callback();
    }

    private initPlayer(): void {
        this.initMediaPlayers();
        this.initPlayerControls();
        loadTooltips(this.playerView);
        this.reload(() => this.loadItem());
    }

    private initMediaPlayers(): void {
        const buttons = HtmlControls.Buttons(),
            controls = HtmlControls.UIControls();

        $(this.getPlayers()).on('loadedmetadata', e => {
            const currentIndex = this.playerConfiguration.properties.CurrentItemIndex,
                player: HTMLMediaElement = e.currentTarget as HTMLMediaElement;

            if (this.playerConfiguration.properties.SelectedMediaType === MediaTypes.Podcast ||
                this.playerConfiguration.properties.SelectedMediaType === MediaTypes.Television) {
                player.currentTime = parseInt($('[data-play-index="' + currentIndex + '"]').attr('data-current-time'));
            }
        });
        $(this.getPlayers()).on('ended', e => {
            if (!this.canPlayNext()) /*then*/ (e.currentTarget as HTMLMediaElement).currentTime = 0;
            this.audioVisualizer.stop();
            this.updatePlayCount(e.currentTarget as HTMLMediaElement, () => this.loadNext());
            this.updatePlayerProgress(0);
        });
        $(this.getPlayers()).prop('volume', this.playerConfiguration.properties.Volume / 100.0);

        $(this.getPlayers()).on('durationchange', e => {
            const player: HTMLMediaElement = e.currentTarget as HTMLMediaElement;

            $(controls.PlayerSlider).slider('option', 'max', player.duration);
            $(controls.PlayerTime).text(this.getPlaybackTime(player.currentTime, player.duration));
        });

        $(this.getPlayers()).on('timeupdate', e => {
            const player: HTMLMediaElement = e.currentTarget as HTMLMediaElement,
                currentTime: number = Math.floor(player.currentTime);

            this.enableDisablePreviousNext();
            if ($(controls.PlayerSlider).attr('data-slide-started') !== 'true') {
                $(controls.PlayerSlider).slider('value', currentTime);
                $(controls.PlayerTime).text(this.getPlaybackTime(currentTime, player.duration));
                if (currentTime > 0) /*then*/ this.updatePlayerProgress(currentTime);
            }
        });

        $(this.getPlayers()).on('play', e => {
            const mediaType = this.playerConfiguration.properties.SelectedMediaType;

            if (this.getPlayer().duration === Infinity) /*then*/ this.getPlayer().src = this.getPlayer().src;
            $(e.currentTarget).attr('data-playing', 'true');
            $([buttons.PlayerPlayButton, buttons.HeaderPlayButton]).addClass('d-none');
            $([buttons.PlayerPauseButton, buttons.HeaderPauseButton]).removeClass('d-none');
            if (mediaType !== MediaTypes.Television) {
                if (!this.audioVisualizer.isInitialized()) /*then*/ this.audioVisualizer.init();
                this.audioVisualizer.start();
            }
        });

        $(this.getPlayers()).on('pause', e => {
            $([buttons.PlayerPauseButton, buttons.HeaderPauseButton]).addClass('d-none');
            $([buttons.PlayerPlayButton, buttons.HeaderPlayButton]).removeClass('d-none');
            this.audioVisualizer.stop();
        });

        $(this.getPlayers()).on('error', e => null);
    }

    private initPlayerControls(): void {
        const $volumeSlider = $('<div id="volume-slider" class="m-1"></div>'),
            buttons = HtmlControls.Buttons(),
            containers = HtmlControls.Containers(),
            controls = HtmlControls.UIControls();

        $(controls.PlayerSlider).slider({ min: 0, max: 100 });
        $volumeSlider.slider({
            min: 0,
            max: 100,
            orientation: 'vertical',
            value: this.playerConfiguration.properties.Muted ? 0 : this.playerConfiguration.properties.Volume
        });
        $(containers.PlayerVolumeContainer).popover({
            trigger: 'hover',
            content: $volumeSlider[0],
            placement: 'top',
            html: true,
            container: containers.PlayerVolumeContainer
        });
        $volumeSlider.on('slide', (e, ui) => {
            const volume = ui.value;

            $([buttons.PlayerVolumeButton, buttons.PlayerMuteButton]).attr('data-volume', volume).addClass('d-none');
            $(volume === 0 ? buttons.PlayerMuteButton : buttons.PlayerVolumeButton).removeClass('d-none');
            this.playerConfiguration.properties.Volume = volume;
            this.playerConfiguration.properties.Muted = volume === 0;
            $(this.getPlayers()).prop('volume', volume / 100.0).prop('muted', volume === 0)
        });
        $volumeSlider.on('slidechange', (e, ui) => {
            this.playerConfiguration.updateConfiguration();
        });
        $(controls.PlayerSlider).on('slide', (e, ui) => {
            if ($(e.currentTarget).attr('data-slide-started') === 'true') {
                $(this.getPlayer()).prop('currentTime', ui.value);
                $(controls.PlayerTime).text(this.getPlaybackTime(ui.value, $(e.currentTarget).slider('option', 'max')));
            }
        });
        $(controls.PlayerSlider).on('slidestart', (e, ui) => $(e.currentTarget).attr('data-slide-started', 'true'));
        $(controls.PlayerSlider).on('slidestop', (e, ui) => $(e.currentTarget).attr('data-slide-started', 'false'));
        $([buttons.HeaderNextButton, buttons.PlayerNextButton]).on('click', () => this.loadNext());
        $([buttons.HeaderPreviousButton, buttons.PlayerPreviousButton]).on('click', () => this.loadPrevious());
        $([buttons.HeaderBackwardButton, buttons.PlayerBackwardButton]).on('click', () => this.skipBackward());
        $([buttons.HeaderForwardButton, buttons.PlayerForwardButton]).on('click', () => this.skipForward());
        $([buttons.HeaderPauseButton, buttons.PlayerPauseButton]).on('click', () => $(this.getPlayer()).attr('data-playing', 'false').trigger('pause'));
        $([buttons.HeaderPlayButton, buttons.PlayerPlayButton]).on('click', () => {
            if (this.getPlayer().currentSrc) /*then*/ $(this.getPlayer()).trigger('play');
        });
        $([buttons.HeaderShuffleButton, buttons.PlayerShuffleButton]).addClass(this.playerConfiguration.properties.Shuffle ? 'active' : '');
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

            $('button[data-repeat-type="' + getRepeatTypesEnumString(repeat) + '"]').removeClass('d-none');
            this.playerConfiguration.properties.Repeat = repeat;
            this.playerConfiguration.updateConfiguration(() => this.enableDisablePreviousNext());
        });
        $([buttons.HeaderShuffleButton, buttons.PlayerShuffleButton]).on('click', () => {
            const shuffle = this.playerConfiguration.properties.Shuffle,
                $btns = $([buttons.HeaderShuffleButton, buttons.PlayerShuffleButton]);

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
        $([buttons.PlayerMuteButton, buttons.PlayerVolumeButton]).on('click', e => {
            const previousVolume = parseInt($(buttons.PlayerVolumeButton).attr('data-volume')),
                $btn = $(e.currentTarget);
            let muted = false;

            $([buttons.PlayerMuteButton, buttons.PlayerVolumeButton]).addClass('d-none');

            if ($btn.attr('id') === buttons.PlayerVolumeButton.id) {
                $(buttons.PlayerMuteButton).removeClass('d-none');
                $volumeSlider.slider('value', 0);
                muted = true;
            } else if ($btn.attr('id') === buttons.PlayerMuteButton.id) {
                $(buttons.PlayerVolumeButton).removeClass('d-none');
                $volumeSlider.slider('value', previousVolume);
            }

            this.playerConfiguration.properties.Muted = muted;
            this.playerConfiguration.updateConfiguration(() => $(this.getPlayers()).each((index, element) => { (element as HTMLAudioElement).muted = muted; }));
        });
        $(buttons.PlayerFullscreenButton).on('click', () => openFullscreen(this.getPlayer()));
        $('button[data-repeat-type="' + getRepeatTypesEnumString(this.playerConfiguration.properties.Repeat) + '"]').removeClass('d-none');
        $(buttons.PlayerPlaylistToggleButton).on('click', e => {
            const $player = $(this.getPlayer()),
                $playerItems = $(containers.PlayerItemsContainer),
                $btn = $(e.currentTarget);
            let page = this.playerConfiguration.properties.SelectedPlayerPage;

            $(buttons.PlayerFullscreenButton).addClass('d-none');
            if (page === PlayerPages.Index) {
                this.playerConfiguration.properties.SelectedPlayerPage = getPlayerPageEnum($player.attr('data-player-page'));
                $player.parent().removeClass('d-none');
                $playerItems.addClass('d-none');
                $btn.removeClass('active');
                page = this.playerConfiguration.properties.SelectedPlayerPage;
                if (page === PlayerPages.Video) /*then*/ $(buttons.PlayerFullscreenButton).removeClass('d-none');
                else if (page === PlayerPages.Audio) /*then*/ this.audioVisualizer.prepareCanvas();
            } else {
                this.playerConfiguration.properties.SelectedPlayerPage = PlayerPages.Index;
                $player.parent().addClass('d-none');
                $playerItems.removeClass('d-none');
                $btn.addClass('active');
            }
            this.playerConfiguration.updateConfiguration();
        });
        $(buttons.PlayerAudioVisualizerButton).on('click', e => {
            const button: HTMLElement = e.currentTarget;

            if (!this.audioVisualizer.isInitialized()) /*then*/ this.audioVisualizer.init();

            if ($(button).hasClass('active')) {
                this.playerConfiguration.properties.AudioVisualizerEnabled = false;
                this.playerConfiguration.updateConfiguration(() => {
                    $(button).removeClass('active');
                    this.audioVisualizer.disable();
                });
            } else {
                this.playerConfiguration.properties.AudioVisualizerEnabled = true;
                this.playerConfiguration.updateConfiguration(() => {
                    $(button).addClass('active');
                    this.audioVisualizer.enable();

                    if (this.isPlaying()) /*then*/ this.audioVisualizer.start();
                });
            }
        });
    }

    private loadItem(item: HTMLElement = null, triggerPlay: boolean = false): void {
        const $player = $(this.getPlayer()),
            shuffleEnabled = this.playerConfiguration.properties.Shuffle,
            fields = HtmlControls.UIFields();

        $(this.getPlayers()).prop('src', '').attr('data-item-id', '');

        if (item) {
            const $item = $(item),
                url = $item.attr('data-play-url'),
                index = parseInt($item.attr('data-play-index')),
                id = $item.attr('data-item-id'),
                title = $item.attr('data-title') || '';

            $('li[data-play-index].list-group-item').removeClass('active');
            this.playerConfiguration.properties.CurrentItemIndex = index;
            this.playerConfiguration.updateConfiguration(() => {
                $item.addClass('active');
                $player.attr('data-item-id', id);
                this.currentlyLoadedId = parseInt(id);
                $(fields.NowPlayingTitle).text(title.length > 0 ? ': ' + title : title);
                if (shuffleEnabled && $.inArray(index, this.unPlayedShuffleIds) >= 0) /*then*/ this.unPlayedShuffleIds.splice(this.unPlayedShuffleIds.indexOf(index), 1);
                this.updateScrollTop();
                $player.prop('src', url);
                this.updateActiveMedia();
                this.audioVisualizer.stop();
                if (triggerPlay) {
                    if (this.playerConfiguration.properties.SelectedMediaType === MediaTypes.Television &&
                        this.playerConfiguration.properties.SelectedPlayerPage === PlayerPages.Index) {
                        $(HtmlControls.Buttons().PlayerPlaylistToggleButton).trigger('click');
                    }
                    $player.trigger('play');
                }
                this.enableDisablePreviousNext();
            });
        } else if ($('li[data-play-index].active').length === 1) {
            this.loadItem($('li[data-play-index].active')[0], triggerPlay);
        }
    }

    private loadNext(): void {
        const shuffle = this.playerConfiguration.properties.Shuffle,
            nextIndex = shuffle ? this.unPlayedShuffleIds[getRandomInteger(0, this.unPlayedShuffleIds.length - 1)] :
                this.playerConfiguration.properties.CurrentItemIndex + 1,
            repeat = this.playerConfiguration.properties.Repeat,
            shuffleEmpty = this.unPlayedShuffleIds.length == 0;
        let $item = null;

        if (repeat === RepeatTypes.RepeatOne) {
            $(this.getPlayer()).prop('currentTime', 0);
            if (this.isPlaying()) /*then*/ this.getPlayer().play();
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
                $(HtmlControls.Buttons().PlayerPauseButton).trigger('click');
                this.enableDisablePreviousNext();
            }
        }
    }

    private loadPrevious(): void {
        const shuffle = this.playerConfiguration.properties.Shuffle,
            previousIndex = shuffle ? this.unPlayedShuffleIds[getRandomInteger(0, this.unPlayedShuffleIds.length - 1)] :
                this.playerConfiguration.properties.CurrentItemIndex - 1,
            $item = $('li[data-play-index="' + previousIndex + '"]'),
            player = this.getPlayer(),
            shuffleEmpty = this.unPlayedShuffleIds.length === 0,
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
        const adjustedTime = Number.isNaN(time) || !Number.isFinite(time) ? 0 : time,
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

        this.unPlayedShuffleIds = shuffle && $items.length > 0 ? $.makeArray($items.map((index, element) => parseInt($(element).attr('data-play-index')))) : [];
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

        $.post('Player/UpdatePlayCount', { mediaType: this.playerConfiguration.properties.SelectedMediaType, id: id }, callback);
    }

    private reload(callback: () => void = () => null): void {
        const containers = HtmlControls.Containers(),
            success = () => {
                loadTooltips(containers.PlayerItemsContainer);
                this.applyLoadFunctions();
                this.updateSelectedPlayerPage();
                $(containers.PlayerItemsContainer).find('[data-item-index]').on('click', e => {
                    const index = parseInt($(e.currentTarget).attr('data-item-index')),
                        item: HTMLElement = $(containers.PlayerItemsContainer).find('li[data-play-index="' + index + '"]')[0] as HTMLElement;

                    this.loadItem(item, true);
                });

                if (typeof callback === 'function') /*then*/ callback();
            };

        disposeTooltips(containers.PlayerItemsContainer);
        $(HtmlControls.UIFields().NowPlayingTitle).text('');
        $(containers.PlayerItemsContainer).html('');
        $(containers.PlayerItemsContainer).load('Player/GetPlayerItems', success);
    }

    private applyLoadFunctions(): void {
        $(this.playerView).find('*[data-artist-id]').on('click', e => this.loadFunctions.loadArtist(parseInt($(e.currentTarget).attr('data-artist-id'))));
        $(this.playerView).find('*[data-album-id]').on('click', e => this.loadFunctions.loadAlbum(parseInt($(e.currentTarget).attr('data-album-id'))));
        $(this.playerView).find('*[data-podcast-id]').on('click', e => this.loadFunctions.loadPodcast(parseInt($(e.currentTarget).attr('data-podcast-id'))));
        $(this.playerView).find('*[data-series-id]').on('click', e => this.loadFunctions.loadSeries(parseInt($(e.currentTarget).attr('data-series-id'))));
    }

    private updateSelectedPlayerPage(): void {
        const buttons = HtmlControls.Buttons(),
            selectedMediaType = this.playerConfiguration.properties.SelectedMediaType;
        let selectedPlayerPage = this.playerConfiguration.properties.SelectedPlayerPage;

        if (selectedMediaType === MediaTypes.Television && selectedPlayerPage === PlayerPages.Audio) {
            this.playerConfiguration.properties.SelectedPlayerPage = PlayerPages.Video;
        } else if (selectedMediaType !== MediaTypes.Television && selectedPlayerPage === PlayerPages.Video) {
            this.playerConfiguration.properties.SelectedPlayerPage = PlayerPages.Audio;
        }

        if (selectedPlayerPage !== this.playerConfiguration.properties.SelectedPlayerPage) {
            selectedPlayerPage = this.playerConfiguration.properties.SelectedPlayerPage;
            $(buttons.PlayerFullscreenButton).addClass('d-none');

            this.playerConfiguration.updateConfiguration(() =>
                $(this.getPlayers()).each((index, element) => {
                    const page = $(element).attr('data-player-page');

                    if (getPlayerPageEnum(page) === selectedPlayerPage) /*then*/ $(element).parent().removeClass('d-none');
                    else $(element).parent().addClass('d-none');
                })
            );

            if (selectedPlayerPage === PlayerPages.Video) /*then*/ $(buttons.PlayerFullscreenButton).removeClass('d-none');
        }
    }

    play(btn: HTMLButtonElement, playSingleItem: boolean = false, loadPlayer: () => void = () => null): void {
        const $playButtons = $('button[data-play-id]'),
            $playGroups = $('div[data-play-ids]'),
            success = () => this.reload(() => {
                this.loadItem(null, true);
                if (this.playerConfiguration.properties.SelectedMediaType === MediaTypes.Television) /*then*/ loadPlayer();
                LoadingModal.hideLoading();
            }),
            mediaType = $(btn).attr('data-media-type') || getMediaTypesEnumString(MediaTypes.Song),
            data = new FormData();
        let $playData = null;

        LoadingModal.showLoading();

        if (playSingleItem) {
            $playData = $([{ Id: 0, Value: parseInt($(btn).attr('data-play-id')), IsSelected: true }]);
        }
        else if ($playGroups.length > 0) {
            $playData = $playGroups.map((index, element) => ($(element).attr('data-play-ids').split(',')))
                .map((index, element) => ({
                    Id: index,
                    Value: parseInt(element),
                    IsSelected: $(btn).attr('data-play-id') === element
                })
                );
        } else {
            $playData = $playButtons.map((index, _btn) => ({
                Id: index,
                Value: parseInt($(_btn).attr('data-play-id')),
                IsSelected: btn.isSameNode(_btn)
            }));
        }
        data.append('mediaType', mediaType);
        data.append('itemsJSON', JSON.stringify($playData.get()));
        this.playerConfiguration.properties.SelectedMediaType = getMediaTypesEnum(mediaType);
        this.playerConfiguration.updateConfiguration(function () {
            $.ajax({
                type: 'POST',
                url: 'Player/UpdateNowPlaying',
                data: data,
                contentType: false,
                success: success,
                processData: false,
                traditional: true
            });
        });
    }

    public getCurrentlyLoadedId(): number {
        return this.currentlyLoadedId;
    }

    private updatePlayerProgress(progress: number): void {
        const id: number = parseInt($('[data-play-index="' + this.playerConfiguration.properties.CurrentItemIndex + '"]').attr('data-item-id')),
            mediaType: MediaTypes = this.playerConfiguration.properties.SelectedMediaType,
            data = {
                id: id, mediaType: mediaType, progress: progress
            },
            currentIndex: number = this.playerConfiguration.properties.CurrentItemIndex;

        if ($('[data-play-index="' + currentIndex + '"]').attr('data-current-time') !== progress.toString() &&
            progress % 5 === 0) {
            $('[data-play-index="' + currentIndex + '"]').attr('data-current-time', progress);
            $.post('Player/UpdatePlayerProgress', data);
        }
    }

    private skipForward(): void {
        const player: HTMLMediaElement = this.getPlayer(),
            currentTime: number = player.currentTime,
            updatedTime: number = currentTime + this.playerConfiguration.properties.SkipForwardSeconds;

        if (updatedTime >= player.duration) {
            player.currentTime = player.duration;
        } else {
            player.currentTime = updatedTime;
        }
    }

    private skipBackward(): void {
        const player: HTMLMediaElement = this.getPlayer(),
            currentTime: number = player.currentTime,
            updatedTime: number = currentTime - this.playerConfiguration.properties.SkipForwardSeconds;

        if (updatedTime < 0) {
            player.currentTime = 0;
        } else {
            player.currentTime = updatedTime;
        }
    }
}