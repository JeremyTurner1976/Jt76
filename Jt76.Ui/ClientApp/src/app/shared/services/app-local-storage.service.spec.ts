import { TestBed, inject } from '@angular/core/testing';
import { AppLocalStorageService } from './app-local-storage.service';

describe('AppLocalStorageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AppLocalStorageService]
    });
  });

  it('should be created', inject([AppLocalStorageService], (service: AppLocalStorageService) => {
    expect(service).toBeTruthy();
  }));
});
