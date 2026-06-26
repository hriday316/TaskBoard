import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface User {
    id: string;
    userName: string;
    email: string;
    profilePictureUrl: string | null;
}
 
@Injectable({
  providedIn: 'root'
})
export class AuthService {
    baseUrl = environment.apiUrl;
    private http = inject(HttpClient);
    currentUser = signal<User | null>(null);

    register( value: { name: string; email: string; password: string }): Observable<any> {
        return this.http.post(this.baseUrl + 'auth/register', value);
    }

    login(value: { email: string; password: string }): Observable<User> {
        return this.http.post<User>(this.baseUrl + 'auth/login', value, { withCredentials: true }).pipe(
            tap((response) => {
                this.currentUser.set(response);
            })
        );
    }
    getCurrentUser(): Observable<User> {
        return this.http.get<User>(this.baseUrl + 'user/me', { withCredentials: true }).pipe(
            tap((response) => {
                this.currentUser.set(response);
            })
        );

}
}