import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { LoadingService } from 'src/app/core/services/loading.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  public username: string;
  public password: string;
  public loading: boolean;
  public error: string;

  constructor(private authenticationService: AuthenticationService, private router: Router, private loadingService: LoadingService) {}

  ngOnInit() {
    this.loadingService.getLoadingObservable().subscribe((loading) => (this.loading = loading));
  }

  public async onLogin(): Promise<void> {
    this.error = '';
    this.loading = true;

    try {
      await this.authenticationService.login(this.username, this.password);
      this.router.navigateByUrl('/');
    } catch (error) {
      this.error = error;
    } finally {
      this.loading = false;
    }
  }
}
