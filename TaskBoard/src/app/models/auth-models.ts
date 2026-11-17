export interface RegisterRequestDto {
    Email: string;
    Password: string;
}

export interface LoginRequestDto {
    Email: string;
    Password: string;
}

export interface LoginResponseDto {
  accessToken: string;
  expiresAtUtc: string;
}