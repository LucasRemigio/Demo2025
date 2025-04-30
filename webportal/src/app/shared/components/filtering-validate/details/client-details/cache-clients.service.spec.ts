import { TestBed } from '@angular/core/testing';

import { CacheClientsServiceService } from './cache-clients.service';

describe('CacheClientsServiceService', () => {
    let service: CacheClientsServiceService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(CacheClientsServiceService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
