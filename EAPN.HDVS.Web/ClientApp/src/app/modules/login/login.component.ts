import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { LoadingService } from 'src/app/core/services/loading.service';
import { finalize } from 'rxjs/operators';

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
  private returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private authenticationService: AuthenticationService,
    private router: Router,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.loadingService.getLoadingObservable().subscribe((loading) => (this.loading = loading));
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  public onLogin(): void {
    this.error = '';
    this.loading = true;

    this.authenticationService
      .login(this.username, this.password)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(
        (result) => (result ? this.router.navigateByUrl(this.returnUrl) : (this.error = 'no result')),
        (err) => (this.error = err)
      );
  }
}
