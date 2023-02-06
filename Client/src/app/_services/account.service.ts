import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UrlSegment } from '@angular/router';
import { map, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  //-a 'baseUrl' foi definida no arquivo 'environment.ts'.
  baseUrl= environment.apiUrl;
  
  //-funciona como um buffer, quando é feito um subscribe, ele retorna a quantidade prédeterminada de valores armazenados automáticamente.
  //-neste caso o buffer é 1.
  private currentUserSource= new ReplaySubject<User>(1);

  //-variável do tipo observable que vai receber o valor armazenado no buffer currentUserSource.
  currentUser$= this.currentUserSource.asObservable();

  //-a resposta da api é um 'UserDto'.
  login(model:any){
    return this.http.post(this.baseUrl + "account/login", model)
    .pipe(
      map((response:User)=>{
        const user= response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post(this.baseUrl + "account/register", model)
    .pipe(
      map((user:User)=>{
        if(user){
          this.setCurrentUser(user);          
        }
        return user;
      })
    )
  }

  setCurrentUser(user:User){
    user.roles= [];
    const roles= this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles= roles : user.roles.push(roles);
    //-o 'user' é o nome da chave que será armazenada no localStorage, o valor da chave é JSON.stringify(user).
    localStorage.setItem('user', JSON.stringify(user));
    //-armazenar no buffer o usuário logado.
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  getDecodedToken(token:string){
    return JSON.parse(atob(token.split('.')[1]))
  }

}
