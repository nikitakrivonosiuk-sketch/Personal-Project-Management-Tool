import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  private readonly tokenKey = 'access_token';

  setToken(token: string): void {
    sessionStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  clearToken(): void {
    sessionStorage.removeItem(this.tokenKey);
  }

  getUserId(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payloadPart = token.split('.')[1];
      if (!payloadPart) return null;

      const base64 = payloadPart
        .replace(/-/g, '+')
        .replace(/_/g, '/')
        .padEnd(Math.ceil(payloadPart.length / 4) * 4, '=');

      const payload = JSON.parse(atob(base64)) as { sub?: string };
      return payload.sub ?? null;
    } catch {
      return null;
    }
  }
}
