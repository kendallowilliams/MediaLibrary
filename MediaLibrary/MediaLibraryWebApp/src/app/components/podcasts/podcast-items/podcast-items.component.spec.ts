import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PodcastItemsComponent } from './podcast-items.component';

describe('PodcastItemsComponent', () => {
  let component: PodcastItemsComponent;
  let fixture: ComponentFixture<PodcastItemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PodcastItemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PodcastItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
