package main

import (
	"github.com/gorilla/mux"
	"net/http"
)

func main() {
	router := mux.NewRouter().PathPrefix("/api/v1/").Subrouter()
	router.HandleFunc("/containers", getContainers).Methods("GET")
	router.HandleFunc("/container/{id}", getContainerWithID).Methods("GET")
	router.HandleFunc("/container/{id}/logs", getLogsWithID).Methods("GET")
	router.HandleFunc("/container/{id}/logs/{tail}", getLogsWithIDAndTail).Methods("GET")

	http.Handle("/", router)
	http.ListenAndServe(":8000", nil)
}
