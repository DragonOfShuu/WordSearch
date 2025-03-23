import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordsToFindComponent } from './words-to-find.component';

describe('WordsToFindComponent', () => {
  let component: WordsToFindComponent;
  let fixture: ComponentFixture<WordsToFindComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WordsToFindComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordsToFindComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
