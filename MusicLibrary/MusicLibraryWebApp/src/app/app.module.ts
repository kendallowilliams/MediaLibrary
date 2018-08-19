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

import { ArtistsResolver } from './components/music/resolvers/artists.resolver';
import { GenresResolver } from './components/music/resolvers/genres.resolver';
import { ControlsComponent } from './components/controls/controls.component';
import { TrackListComponent } from './components/music/track-list/track-list.component';
import { AlbumListComponent } from './components/music/album-list/album-list.component';
import { ArtistListComponent } from './components/music/artist-list/artist-list.component';
import { MusicSettingsComponent } from './components/music/music-settings/music-settings.component';
import { MlRouteReuseStrategy } from './route_reuse_strategies/route-reuse-strategy';

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
    MusicSettingsComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
        { path: '', redirectTo: 'app-music', pathMatch: 'full' },
        { path: 'app-music', component: MusicComponent,
          resolve: {
           artists: ArtistsResolver
          }
        },
        { path: 'app-nowplaying', component: NowPlayingComponent },
        { path: 'app-podcasts', component: PodcastsComponent },
        { path: 'app-settings', component: SettingsComponent },
        { path: 'app-playlists', component: PlaylistsComponent },
        { path: '**', redirectTo: 'app-music' }
    ]),
    HttpClientModule
  ],
  providers: [ ArtistsResolver, GenresResolver,
    { provide: RouteReuseStrategy, useClass: MlRouteReuseStrategy }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
