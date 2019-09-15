import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoMohamyComponent } from './videomohamy.component';

describe('Gendy1Component', () => {
  let component: VideoMohamyComponent;
  let fixture: ComponentFixture<VideoMohamyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VideoMohamyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VideoMohamyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
