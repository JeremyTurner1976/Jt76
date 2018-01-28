import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "Jt76.Ui";
  themeClass: string;
  isAuthenticated = false;

  ngOnInit(): void {
    this.themeClass = "dark-theme";
  }

  setTheme(theme) {
    console.log(theme);
    console.log(theme);
    this.themeClass = theme + "-theme";
  }

  onLoginClick() {
    this.isAuthenticated = true;
  }

  onLogoutClick() {
    this.isAuthenticated = false;
  }
}
