// UserDto
export interface User {
  id: string;
  username: string;
  email: string;
  avatarFileName: string | null;
  isAdmin: boolean;
  createdAt: string;
}

// LoginUserDto
export interface LoginRequest {
  login: string;
  password: string;
}

// RegisterUserDto
export interface RegisterRequest {
  username: string;
  email: string;
  passwordHash: string;
  avatarUrl: string | null;
}

// LoginResultDto
export interface AuthResponse {
  token: string;
  user: User;
}
