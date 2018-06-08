import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MusicSettingsComponent } from './music-settings.component';

describe('MusicSettingsComponent', () => {
  let component: MusicSettingsComponent;
  let fixture: ComponentFixture<MusicSettingsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MusicSettingsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MusicSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
