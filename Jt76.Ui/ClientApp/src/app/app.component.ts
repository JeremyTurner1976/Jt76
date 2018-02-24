import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"]
})
export class AppComponent implements OnInit {
  title = "Jt76.Ui";
  theme: string;
  themeClass: string;
  isAuthenticated = false;
  year = new Date().getFullYear();

  ngOnInit(): void {
    this.setTheme("default");
  }

  setTheme(theme) {
    this.theme = theme;
    this.themeClass = this.colorTheme();
  }

  colorTheme() {
    return `${this.theme}-theme`;
  }

  contentTheme() {
    return `${this.theme}-colors content`;
  }

  onLoginClick() {
    this.isAuthenticated = true;
  }

  onLogoutClick() {
    this.isAuthenticated = false;
  }
}
