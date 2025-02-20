import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingleplayerUiComponent } from './singleplayer-ui.component';

describe('SingleplayerUiComponent', () => {
  let component: SingleplayerUiComponent;
  let fixture: ComponentFixture<SingleplayerUiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
    imports: [SingleplayerUiComponent],
}).compileComponents();

    fixture = TestBed.createComponent(SingleplayerUiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
