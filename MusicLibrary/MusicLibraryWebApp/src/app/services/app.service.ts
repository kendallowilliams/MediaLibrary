import { Injectable } from '../../../node_modules/@angular/core';
import { MusicComponent } from '../components/music/music.component';
import { ControlsComponent } from '../components/controls/controls.component';
import { NavmenuComponent } from '../components/navmenu/navmenu.component';

@Injectable({
  providedIn: 'root'
})

export class AppService {
  private _musicComponent: MusicComponent;
  private _controlsComponent: ControlsComponent;

  constructor() { }

  get musicComponent(): MusicComponent {
    return this._musicComponent;
  }

  set musicComponent(component: MusicComponent) {
    this._musicComponent = component;
  }

  get controlsComponent(): ControlsComponent {
    return this._controlsComponent;
  }

  set controlsComponent(component: ControlsComponent) {
    this._controlsComponent = component;
  }
}
