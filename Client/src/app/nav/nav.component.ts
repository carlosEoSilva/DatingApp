import { Component, OnInit } from '@angular/core';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
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

  constructor(private accountService:AccountService ) { }

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
        console.log(response);
        this.loggedIn= true;
      },
      error: error => console.error(error)
    });
  }

  logout(){
    this.accountService.logout();
  }

}
