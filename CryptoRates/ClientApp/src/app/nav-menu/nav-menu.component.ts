import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../core/services/authentication.service';
import { User } from '../core/models/user';
import { Role } from '../core/models/role';

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

    get isAdmin() {
        return this.currentUser && this.currentUser.role === Role.Admin;
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}
