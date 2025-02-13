import { TestBed } from '@angular/core/testing';

import { SignalRSocketService } from './signal-rsocket.service';

describe('SignalRSocketService', () => {
  let service: SignalRSocketService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalRSocketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
