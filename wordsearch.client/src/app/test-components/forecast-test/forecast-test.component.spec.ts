import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForecastTestComponent } from './forecast-test.component';

describe('ForecastTestComponent', () => {
  let component: ForecastTestComponent;
  let fixture: ComponentFixture<ForecastTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ForecastTestComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ForecastTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
