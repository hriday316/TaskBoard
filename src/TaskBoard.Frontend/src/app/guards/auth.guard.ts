import { CanActivateFn, Router } from '@angular/router';
 import { inject } from '@angular/core/primitives/di';
import { AuthService } from '../services/auth.service';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';


export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.currentUser()) {
    return true;
  }
  return firstValueFrom(authService.getCurrentUser()).then((user) => {
    if (user) {
      authService.currentUser.set(user);
      return true;
    } else {
      return false;
    }
  }).catch((error) => {
    router.navigate(['/login']);
     return false;
  });
};
