import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiplayerMessagerComponent } from './multiplayer-messager.component';

describe('MultiplayerMessagerComponent', () => {
  let component: MultiplayerMessagerComponent;
  let fixture: ComponentFixture<MultiplayerMessagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MultiplayerMessagerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MultiplayerMessagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
