import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TrackRowComponent } from './track-row.component';

describe('TrackComponent', () => {
  let component: TrackRowComponent;
  let fixture: ComponentFixture<TrackRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrackRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrackRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
