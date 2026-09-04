import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';

describe('AuthService', () => {
  let service: AuthService;
  let http: HttpTestingController;

  beforeEach(() => {
    sessionStorage.clear();
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    service = TestBed.inject(AuthService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => { http.verify(); sessionStorage.clear(); });

  it('stores the access token after login', () => {
    service.login('demo@taskmanager.local', 'Demo1234').subscribe();
    const request = http.expectOne('/api/auth/login');
    request.flush({
      userId: '9b9df3aa-ef15-4384-a66c-60488c752e0e',
      name: 'Demo User',
      email: 'demo@taskmanager.local',
      accessToken: 'signed-token',
      expiresAt: '2026-09-03T00:00:00Z',
    });
    expect(service.token()).toBe('signed-token');
    expect(service.isAuthenticated()).toBe(true);
  });
});
