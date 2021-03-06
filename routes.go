package main

import (
	"bufio"
	"context"
	"encoding/json"
	"fmt"
	"github.com/docker/docker/api/types"
	"github.com/docker/docker/client"
	"github.com/gorilla/mux"
	"net/http"
	"strings"
	"time"
)

func getCLI() *client.Client {
	cli, err := client.NewClientWithOpts(client.FromEnv, client.WithAPIVersionNegotiation())
	if err != nil {
		panic(err)
	}

	return cli
}

func getContainers(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")

	cli := getCLI()
	containers, err := cli.ContainerList(context.Background(), types.ContainerListOptions{
		All: true,
	})
	if err != nil {
		panic(err)
	}

	json.NewEncoder(w).Encode(containers)
}

func getContainerWithID(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)

	cli := getCLI()
	containers, err := cli.ContainerList(context.Background(), types.ContainerListOptions{
		All: true,
	})
	if err != nil {
		panic(err)
	}

	for _, container := range containers {
		if container.ID == params["id"] {
			json.NewEncoder(w).Encode(container)
		}
	}
}

func getLogsWithID(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)

	f, ok := w.(http.Flusher)
	if !ok {
		http.Error(w, "Streaming unsupported!", http.StatusInternalServerError)
		return
	}

	cli := getCLI()
	logs, err := cli.ContainerLogs(context.Background(), params["id"], types.ContainerLogsOptions{
		ShowStdout: true,
		Follow:     true,
	})
	if err != nil {
		panic(err)
	}

	buffered := bufio.NewReader(logs)
	for {
		message, err := buffered.ReadString('\n')
		if err != nil {
			panic(err)
		}

		fmt.Fprintf(w, "%s\n", message)
		if index := strings.IndexAny(message, " "); index != -1 {
			id := message[:index]
			if _, err := time.Parse(time.RFC3339Nano, id); err == nil {
				_, err := fmt.Fprintf(w, "%s\n", id)
				if err != nil {
					panic(err)
				}
			}
		}

		f.Flush()
	}
}

func getLogsWithIDAndTail(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)

	f, ok := w.(http.Flusher)
	if !ok {
		http.Error(w, "Streaming unsupported!", http.StatusInternalServerError)
		return
	}

	cli := getCLI()
	logs, err := cli.ContainerLogs(context.Background(), params["id"], types.ContainerLogsOptions{
		ShowStdout: true,
		Follow:     true,
		Tail:       params["tail"],
	})
	if err != nil {
		panic(err)
	}

	buffered := bufio.NewReader(logs)
	for {
		message, err := buffered.ReadString('\n')
		if err != nil {
			panic(err)
		}

		fmt.Fprintf(w, "%s\n", message)
		if index := strings.IndexAny(message, " "); index != -1 {
			id := message[:index]
			if _, err := time.Parse(time.RFC3339Nano, id); err == nil {
				_, err := fmt.Fprintf(w, "%s\n", id)
				if err != nil {
					panic(err)
				}
			}
		}

		f.Flush()
	}
}

func getInspectWithID(w http.ResponseWriter, r *http.Request) {
	w.Header().Set("Content-Type", "application/json")
	params := mux.Vars(r)

	cli := getCLI()
	inspect, err := cli.ContainerInspect(context.Background(), params["id"])
	if err != nil {
		panic(err)
	}

	json.NewEncoder(w).Encode(inspect)
}
