import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleplayerCommsComponent } from './singleplayer-comms.component';

describe('SingleplayerCommsComponent', () => {
  let component: SingleplayerCommsComponent;
  let fixture: ComponentFixture<SingleplayerCommsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SingleplayerCommsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SingleplayerCommsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
