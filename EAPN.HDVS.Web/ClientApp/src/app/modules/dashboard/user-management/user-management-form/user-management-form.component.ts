import { Component, OnInit } from '@angular/core';
import { UserManagementService } from 'src/app/core/services/user-management.service';
import { AlertService } from 'src/app/core/services/alert.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UsuarioDto } from 'src/app/core/api/api.client';

@Component({
  selector: 'app-user-management-form',
  templateUrl: './user-management-form.component.html',
  styleUrls: ['./user-management-form.component.scss'],
})
export class UserManagementFormComponent implements OnInit {
  public title: string;
  public user: UsuarioDto;

  constructor(
    private userService: UserManagementService,
    private alertService: AlertService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const snapshot = this.route.snapshot;
    const userId = snapshot.params['id'];
    if (!!userId) {
      this.title = userId;
    } else {
      this.title = 'Nuevo Usuario';
      this.user = new UsuarioDto();
    }
  }
}
