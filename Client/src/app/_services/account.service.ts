import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, ReplaySubject } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  baseUrl= "https://localhost:5001/api/";
  
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
          //-o 'user' é o nome da chave que será armazenada no localStorage, o valor da chave é JSON.stringify(user).
          localStorage.setItem('user', JSON.stringify(user));
          //-armazenar no buffer o usuário logado.
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post(this.baseUrl + "account/register", model)
    .pipe(
      map((user:User)=>{
        if(user){
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user:User){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

}
