import BaseClass from "../../assets/models/base-class";
import IView from "../../assets/interfaces/view-interface";
import PlayerConfiguration from "../../assets/models/configurations/player-configuration";
import HtmlControls from '../../assets/controls/html-controls'
import { MediaTypes, RepeatTypes } from "../../assets/enums/enums";
import { getRandomInteger } from "../../assets/utilities/math";

export default class Player extends BaseClass implements IView {
    private players: { VideoPlayer: HTMLElement, MusicPlayer: HTMLElement };
    private unPlayedShuffleIds: string[];

    constructor(private playerConfiguration: PlayerConfiguration) {
        super();
        this.players = HtmlControls.Players();
        this.unPlayedShuffleIds = [];
    }

    loadView(): void {
    }

    loadItem(item: HTMLElement, triggerPlay: boolean): void {
        const $player = $(this.getPlayer()),
            shuffleEnabled = this.playerConfiguration.properties.Shuffle;

        $(this.getPlayers()).prop('src', '').attr('data-item-id', '');

        if (item) {
            let $item = $(item),
                url = $item.attr('data-play-url'),
                index = parseInt($item.attr('data-play-index')),
                id = $item.attr('data-item-id'),
                title = $item.attr('data-title') || '';

            $('li[data-play-index].list-group-item').removeClass('active');
            this.playerConfiguration.properties.CurrentItemIndex = index;
            this.playerConfiguration.updateConfiguration(function () {
                $item.addClass('active');
                $player.attr('data-item-id', id);
                $('#@(HtmlControlsRepository.NowPlayingTitleId)').text(title.length > 0 ? ': ' + title : title);
                if (shuffleEnabled && $.inArray(index, this.unPlayedShuffleIds) >= 0) /*then*/ this.unPlayedShuffleIds.splice(this.unPlayedShuffleIds.indexOf(index), 1);
                this.updateScrollTop();
                $player.prop('src', url);
                if (triggerPlay) /*then*/ $player.trigger('play');
                this.enableDisablePreviousNext();
            });
        } else if ($('li[data-play-index].active').length === 1) {
            this.loadItem($('li[data-play-index].active')[0], triggerPlay);
        }
    }

    private loadNext(): void {
        var shuffle = this.playerConfiguration.properties.Shuffle,
            nextIndex = shuffle ? this.unPlayedShuffleIds[getRandomInteger(0, this.unPlayedShuffleIds.length - 1)] :
                this.playerConfiguration.properties.CurrentItemIndex + 1,
            $item = null,
            repeat = this.playerConfiguration.properties.Repeat,
            shuffleEmpty = this.unPlayedShuffleIds.length == 0;

        if (repeat === RepeatTypes.RepeatOne) {
            $(this.getPlayer()).prop('currentTime', 0);
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
                $('#@(HtmlControlsRepository.PlayerPauseButtonId)').trigger('click');
                this.enableDisablePreviousNext();
            }
        }
    }

    private loadPrevious(): void {
        var shuffle = this.playerConfiguration.properties.Shuffle,
            previousIndex = shuffle ? this.unPlayedShuffleIds[getRandomInteger(0, this.unPlayedShuffleIds.length - 1)] :
                this.playerConfiguration.properties.CurrentItemIndex - 1,
            $item = $('li[data-play-index="' + previousIndex + '"]'),
            player = this.getPlayer(),
            shuffleEmpty = this.unPlayedShuffleIds.length == 0,
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

    private enableDisablePreviousNext(): void {

    }

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
        let adjustedTime = Number.isNaN(time) || !Number.isFinite(time) ? 0 : time,
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

        this.unPlayedShuffleIds = shuffle && $items.length > 0 ? $.makeArray($items.map((index, element) => $(element).attr('data-play-index'))) : [];
    }
}