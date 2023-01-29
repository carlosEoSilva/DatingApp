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
  model:any= {};
  //-o valor padrão de um 'boolean' é 'false'.
  loggedIn:boolean;
  userIcon="../../assets/user.png";
  currentUser$:Observable<User>;

  constructor(
    private accountService:AccountService, 
    private router:Router,
    private toastr:ToastrService ) { }

  ngOnInit(): void { 
    this.currentUser$= this.accountService.currentUser$;
    this.test();
  }

  login(){
    this.accountService.login(this.model)
    .subscribe({
      next: response => {
        this.router.navigateByUrl('/members');
        this.loggedIn= true;
      },
      error: error => {
        console.error(error);
      } 
    });
  }

  logout(){
    this.router.navigateByUrl('/');
    this.accountService.logout();
  }

  test(){
    this.currentUser$.subscribe({
      next:(user)=>{
        // console.log(user);
      }
    })
  }

}
