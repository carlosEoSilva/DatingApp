import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: true } }]
})
export class NavComponent implements OnInit {

  constructor(
    private accountService:AccountService, 
    private router:Router,
    private toastr:ToastrService ) { }

  ngOnInit(): void { 
    this.currentUser$= this.accountService.currentUser$;
  }

  model:any= {};
  //-o valor padrão de um 'boolean' é 'false'.
  loggedIn:boolean;

  currentUser$:Observable<User>;

  login(){
    this.accountService.login(this.model)
    .subscribe({
      next: response => {
        // console.log(response);
        this.router.navigateByUrl('/members');
        this.loggedIn= true;
      },
      error: error => {
        console.error(error);
        this.toastr.error(error.error);
      } 
    });
  }

  logout(){
    this.router.navigateByUrl('/');
    this.accountService.logout();
  }

}
