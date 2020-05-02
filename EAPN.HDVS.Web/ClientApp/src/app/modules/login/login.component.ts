import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public username: string;
  public password: string;

  constructor(private authenticationService: AuthenticationService, private router: Router) {}

  ngOnInit() {}

  public onLogin(): void {
    this.authenticationService.login(this.username, this.password).subscribe((result) => this.router.navigateByUrl('/'));
  }
}
