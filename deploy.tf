terraform {
  required_providers {
    google = {
      source  = "hashicorp/google"
      version = "4.64.0"
    }
  }
}

provider "google" {
  # Configuration options
  credentials = file("mygcp-credentials.json")
  project = "mineral-music-331421"
  region  = "us-central1"
  zone = "us-central1-a"
}

resource "google_compute_instance" "my_server" {
  machine_type = "e2-micro"
  name         = "my-gcp-server"
  boot_disk {
    initialize_params {
      image = "debian-11-bullseye-v20230509"
    }
  }
  network_interface {
    network = "default"
    access_config {}
  }
}
