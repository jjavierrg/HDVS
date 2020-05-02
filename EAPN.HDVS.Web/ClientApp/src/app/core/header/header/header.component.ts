import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  public username: Observable<string>;

  constructor(private authService: AuthenticationService, private router: Router) {}

  ngOnInit() {
    this.username = this.authService.getUserObservable().pipe(map((user) => (!!user ? user.email : '')));
  }

  public onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
