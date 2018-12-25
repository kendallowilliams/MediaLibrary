import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, RouteReuseStrategy } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './components/app/app.component';
import { NavmenuComponent } from './components/navmenu/navmenu.component';
import { MusicComponent } from './components/music/music.component';
import { PodcastsComponent } from './components/podcasts/podcasts.component';
import { SettingsComponent } from './components/settings/settings.component';
import { PlaylistsComponent } from './components/playlists/playlists.component';
import { NowPlayingComponent } from './components/nowplaying/nowplaying.component';
import { TrackRowComponent } from './components/music/track-list/track-row/track-row.component';
import { AlbumComponent } from './components/music/album/album.component';
import { ArtistComponent } from './components/music/artist/artist.component';

import { ControlsComponent } from './components/controls/controls.component';
import { TrackListComponent } from './components/music/track-list/track-list.component';
import { AlbumListComponent } from './components/music/album-list/album-list.component';
import { ArtistListComponent } from './components/music/artist-list/artist-list.component';
import { MusicSettingsComponent } from './components/music/music-settings/music-settings.component';
import { MlRouteReuseStrategy } from './route_reuse_strategies/route-reuse-strategy';
import { PodcastComponent } from './components/podcasts/podcast/podcast.component';
import { PodcastItemsComponent } from './components/podcasts/podcast-items/podcast-items.component';
import { PodcastItemComponent } from './components/podcasts/podcast-items/podcast-item/podcast-item.component';

@NgModule({
  declarations: [
    AppComponent,
    NavmenuComponent,
    MusicComponent,
    NowPlayingComponent,
    PlaylistsComponent,
    PodcastsComponent,
    SettingsComponent,
    TrackRowComponent,
    AlbumComponent,
    ArtistComponent,
    ControlsComponent,
    TrackListComponent,
    AlbumListComponent,
    ArtistListComponent,
    MusicSettingsComponent,
    PodcastComponent,
    PodcastItemsComponent,
    PodcastItemComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
        { path: '', redirectTo: 'music', pathMatch: 'full' },
        { path: 'music', component: MusicComponent },
        { path: 'nowplaying', component: NowPlayingComponent },
        { path: 'podcasts', children: [
            { path: '', component: PodcastsComponent },
            { path: ':podcastId', component: PodcastItemsComponent }
          ]
        },
        { path: 'settings', component: SettingsComponent },
        { path: 'playlists', component: PlaylistsComponent },
        { path: '**', redirectTo: 'music' }
    ], { useHash: true }),
    HttpClientModule
  ],
  providers: [{ provide: RouteReuseStrategy, useClass: MlRouteReuseStrategy }],
  bootstrap: [AppComponent]
})
export class AppModule { }
