import { Component } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../core/models/user';
import { UserService} from '../core/services/user.service';
import { AuthenticationService} from '../core/services/authentication.service';

@Component({ templateUrl: 'profile.component.html' })
export class ProfileComponent {
    loading = false;
    currentUser: User;
    userFromApi: User;

    constructor(
        private userService: UserService,
        private authenticationService: AuthenticationService
    ) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    ngOnInit() {
        this.loading = true;
        this.userService.getById(this.currentUser.id).pipe(first()).subscribe(user => {
            this.loading = false;
            this.userFromApi = user;
        });
    }
}
