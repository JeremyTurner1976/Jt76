import { TestBed, inject } from '@angular/core/testing';
import { BaseService } from './base.service';
import { IAppError } from '../../modules/admin/models/app-error';

describe('BaseService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [BaseService]
    });
  });

  it('should be created', inject([BaseService], (service: BaseService<IAppError>) => {
    expect(service).toBeTruthy();
  }));
});
