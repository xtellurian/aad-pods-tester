#!/bin/sh

set -e


# Apply all CRD
kubectl apply -f aad-pod-identity/crd

# Deploy Infra
kubectl apply -f aad-pod-identity/deploy/infra/deployment.yaml

#deploy id made in create-azis.sh
kubectl apply -f srcid/aadpodidentity.yaml

#deploy binding
kubectl apply -f srcid/aadpodidentitybinding.yaml