import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './components/app/app.component';
import { NavmenuComponent } from './components/navmenu/navmenu.component';
import { MusicComponent } from './components/music/music.component';
import { PodcastsComponent } from './components/podcasts/podcasts.component';
import { SettingsComponent } from './components/settings/settings.component';
import { PlaylistsComponent } from './components/playlists/playlists.component';
import { NowPlayingComponent } from './components/nowplaying/nowplaying.component';
import { TrackComponent } from './components/music/track/track.component';
import { AlbumComponent } from './components/music/album/album.component';
import { ArtistComponent } from './components/music/artist/artist.component';

@NgModule({
  declarations: [
    AppComponent,
    NavmenuComponent,
    MusicComponent,
    NowPlayingComponent,
    PlaylistsComponent,
    PodcastsComponent,
    SettingsComponent,
    TrackComponent,
    AlbumComponent,
    ArtistComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot([
        { path: '', redirectTo: 'app-music', pathMatch: 'full' },
        { path: 'app-music', component: MusicComponent },
        { path: 'app-nowplaying', component: NowPlayingComponent },
        { path: 'app-podcasts', component: PodcastsComponent },
        { path: 'app-settings', component: SettingsComponent },
        { path: 'app-playlists', component: PlaylistsComponent },
        { path: '**', redirectTo: 'app-music' }
    ]),
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
