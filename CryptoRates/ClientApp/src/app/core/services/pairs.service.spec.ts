import { TestBed } from '@angular/core/testing';

import { PairsService } from './pairs.service';

describe('PairsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PairsService = TestBed.get(PairsService);
    expect(service).toBeTruthy();
  });
});
