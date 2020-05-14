import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '@app/core/services/authentication.service';
import { User } from '@app/core/models/user';

@Component({ 
    selector: 'nav-menu', 
    templateUrl: 'nav-menu.component.html' 
})

export class NavMenuComponent {
    currentUser: User;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
    }

    get isLoggedIn() {
        return this.authenticationService.isUserLoggedIn;
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}
